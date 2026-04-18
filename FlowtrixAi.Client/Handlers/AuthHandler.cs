using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;

namespace FlowtrixAi.Client.Handlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;
        private readonly IJSRuntime _js;

        public AuthHandler(NavigationManager navigation, IJSRuntime js)
        {
            _navigation = navigation;
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // محاولة الحصول على التوكن من التخزين المحلي
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
            {
                // إضافة التوكن لترويسة الطلب
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // مسح التوكن من التخزين المحلي فوراً
                await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                
                // التوجه لصفحة الدخول
                _navigation.NavigateTo("/login?returnUrl=" + Uri.EscapeDataString(_navigation.Uri), forceLoad: true);
            }

            return response;
        }
    }
}

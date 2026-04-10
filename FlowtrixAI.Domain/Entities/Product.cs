namespace FlowtrixAI.Domain.Entities;

public class Product
{
    public Product()
    {
        BOMs = new HashSet<BillOfMaterial>();
        Processes  = new HashSet<Process>();

    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }


    public ICollection<BillOfMaterial> BOMs { get; set; }
    public ICollection<Process> Processes { get; set; }



}

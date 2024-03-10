namespace WorkFlowLog.Entities
{
    public class Order : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; } //specjalnie generowany numer zlecenia,
                                         //nie jest powiązany z Id obiektu w klasie EntityBase

        public override string ToString() => $"ID: {Id}, Name: {Name}";
    }
}

namespace Gabriel.DesafioFrontEnd.Domain.Entities
{
    public class Customer
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Camera> Cameras { get; set; }

        public Customer() 
            => Cameras = new List<Camera>();
    }
}

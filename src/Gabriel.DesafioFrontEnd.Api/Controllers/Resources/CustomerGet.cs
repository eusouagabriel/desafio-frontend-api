namespace Gabriel.DesafioFrontEnd.Api.Controllers.Resources
{
    public class CustomerGetResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerCamerasGetResponse : CustomerGetResponse
    {
        public ICollection<CameraGetResponse> Cameras { get; set; }
        public CustomerCamerasGetResponse() => Cameras = new List<CameraGetResponse>();
    }

    public class CustomerResponse
    {
        public int TotalRows { get; set; }
        public IEnumerable<CustomerGetResponse> Customers { get; set; }
    }
}

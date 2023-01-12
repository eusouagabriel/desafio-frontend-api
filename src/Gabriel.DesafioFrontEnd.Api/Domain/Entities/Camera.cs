namespace Gabriel.DesafioFrontEnd.Domain.Entities
{
    public class Camera
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Video { get; set; }
        public Customer Customer { get; set; }
    }
}

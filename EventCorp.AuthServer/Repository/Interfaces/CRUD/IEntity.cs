namespace EventCorp.AuthServer.Repository.Interfaces.CRUD
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
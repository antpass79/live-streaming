namespace BlazorProducer.Shared.Models
{
    public class DataChunk<T>
    {
        public int Size { get; set; }
        public T Chunked { get; set; }
    }
}

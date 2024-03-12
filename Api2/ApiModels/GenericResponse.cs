using System;
namespace Api2.ApiModels
{
	public class GenericResponse<T>
	{
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}


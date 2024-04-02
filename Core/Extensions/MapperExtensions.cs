using AutoMapper;

namespace Core.Extensions
{
    public static class MapperExtensions
    {
        public static IMultiMapBuilder<TDestination> MultiMap<TDestination>(this IMapper mapper, object source)
        {
            return new MultiMapBuilder<TDestination>(mapper, source);
        }
    }

    public interface IMultiMapBuilder<T>
    {
        IMultiMapBuilder<T> Then<TSource>(TSource source);
        T Map();
    }

    public class MultiMapBuilder<T> : IMultiMapBuilder<T>
    {
        private readonly IMapper _mapper;
        private readonly T _mappedObject;
        public MultiMapBuilder(IMapper mapper, object source)
        {
            _mapper = mapper;
            _mappedObject = mapper.Map<T>(source);
        }
        public IMultiMapBuilder<T> Then<TSource>(TSource source)
        {
            _mapper.Map(source, _mappedObject);
            return this;
        }

        public T Map()
        {
            return _mappedObject;
        }
    }
}

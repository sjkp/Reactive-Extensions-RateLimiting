using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxForEach
{
    public static class RxExtension
    {
        public static IObservable<T> RateLimit<T>(this IObservable<T> source, TimeSpan minDelay)
        {
            return source.Select(x =>
                Observable.Empty<T>()
                    .Delay(minDelay)
                    .StartWith(x)
            ).Concat();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RxForEach
{
    class Program
    {
        //A random generator so we can have each request take a random amount of time.
        private static Random rng = new Random((int)DateTime.Now.Ticks);
        

        static void Main(string[] args)
        {          
            //Some test data that we are to process   
            var items = new List<User>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add(new User()
                {
                    Name = "Name " + i
                });
            }

            var users = new List<User>().ToObservable();

            items.ToObservable()
                .Select(x => Observable.Defer(() =>
                    someBoolAsyncMethod(x).Select(y => new { Item = x, Result = y })))
                .RateLimit(TimeSpan.FromSeconds(60.0/500.0)) //The the denominator is the maximum number of request you want to send out per second
                .Merge(500) //Number of concurrent requests
                .Subscribe();
                
            Console.Read();
        }

        static IObservable<bool>  someBoolAsyncMethod(User o)
        {
            return Observable.StartAsync<bool>(async () =>
            {
                await Task.Delay(rng.Next(1,5)*1000); //Simulation of a slow HTTP request or some other slow external system that you want to limit the number request to. 
                Console.WriteLine("{0} {1}", DateTime.Now, o.Name);
                return true;
            });
        }
    }

    class User {
        public string Name { get; set; }
    }
}

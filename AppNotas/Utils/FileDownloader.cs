using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Android.Media;
using Newtonsoft.Json.Linq;

namespace AppNotas.Utils
{
	public class FileDownloader
	{
        private MediaPlayer player;
        public FileDownloader()
		{
        }

		public async static void run()
		{
            var client = new HttpClient();
            var res = await client.SendAsync(
                new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://e07.ytjar.info/getMP3Links.v5.php?id=hKX9rEEgb0I&s=1680477702&h=b41d9787c7b9e8fed11f3d99a60aa1e9")
                }
            );
            res.EnsureSuccessStatusCode();
            var audio = await res.Content.ReadAsStringAsync();
            var some = 5;
        }
	}
}


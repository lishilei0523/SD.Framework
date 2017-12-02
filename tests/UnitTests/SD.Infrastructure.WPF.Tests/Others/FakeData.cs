using System.Collections.Generic;

namespace SD.Infrastructure.WPF.Tests.Others
{
    public class FakeData
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public List<FakeData> GenerateFakeSource()
        {
            List<FakeData> source = new List<FakeData>();

            for (int i = 1; i <= 968; i++)
            {
                FakeData item = new FakeData()
                {
                    Id = i,
                    ItemName = "Item" + i
                };

                source.Add(item);
            }

            return source;
        }
    }
}

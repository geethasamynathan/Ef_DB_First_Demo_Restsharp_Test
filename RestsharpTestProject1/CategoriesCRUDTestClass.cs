using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace RestsharpTestProject1
{
    public class CategoriesCRUDTestClass
    {
        private RestClient client;
        private const string BaseUrl = "https://localhost:7047/api/Categories";

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(BaseUrl);
        }

        [Test]    
        public async Task GetCategories_should_Return_ListOfCategories()
        {
            var request = new RestRequest("/",Method.Get);
            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(response.Content);

            var categories=JsonConvert.DeserializeObject<List<Category>>(response.Content);
              Assert.IsInstanceOf<List<Category>>(categories);
        }

        [Test]
        public async Task GetCategory_ShouldReturn_WhenValidId()
        {
            var request=new RestRequest("/2",Method.Get);
            var response= await client.ExecuteAsync(request);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK)); 
            Assert.IsNotNull(response.Content);

            var category=JsonConvert.DeserializeObject<Category>(response.Content);
            Assert.IsNotNull(category);
        }

        [Test]
        public async Task GetCategory_ShouldReturnNotFound_WhenInValidId()
        {
            var request=new RestRequest("/9999",Method.Get);
            var response=await client.ExecuteAsync(request);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task PostCategory_ShouldCreateCategory()
        {
            var  newCategory=new Category() { CategoryName="test" };
            var request=new RestRequest("/",Method.Post);
            request.AddJsonBody(newCategory);

            var response= await client.ExecuteAsync(request);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task PutCategory_ShouldUpdateCategory()
        {
            var updatedCategory=new Category() { CategoryId=4, CategoryName="updated Category" };
            var request = new RestRequest("/4", Method.Put);
            request.AddJsonBody(updatedCategory);

            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task DeleteCategroy_ShouldRemoveCategory()
        {
            var request=new RestRequest("/1008",Method.Delete); 

            var response=await client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}

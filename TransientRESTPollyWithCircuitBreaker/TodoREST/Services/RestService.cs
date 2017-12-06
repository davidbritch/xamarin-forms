using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TodoREST
{
	public class RestService : IRestService
	{
		public List<TodoItem> Items { get; private set; }
        IRequestProvider requestProvider;

        public RestService(IRequestProvider provider)
		{
            requestProvider = provider;
		}

		public async Task<List<TodoItem>> RefreshDataAsync()
		{
			Items = new List<TodoItem>();

            try
            {
                Items = await requestProvider.GetAsync<List<TodoItem>>(string.Format(Constants.RestUrl, string.Empty));
            }
			catch (Exception ex)
			{
			  Debug.WriteLine("ERROR: {0}", ex.Message);
			}

		    return Items;
		}

        public async Task SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
        {
            try
            {
                bool result;
                if (isNewItem)
                {
                    result = await requestProvider.PostAsync<TodoItem>(string.Format(Constants.RestUrl, string.Empty), item);
                }
                else
                {
                    result = await requestProvider.PutAsync<TodoItem>(string.Format(Constants.RestUrl, string.Empty), item);
                }

                if (result)
                {
                    Debug.WriteLine("TodoItem successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: {0}", ex.Message);
            }
        }

		public async Task DeleteTodoItemAsync(string id)
		{
			try
			{
                bool result = await requestProvider.DeleteAsync(string.Format(Constants.RestUrl, id));
				if (result)
				{
					Debug.WriteLine("TodoItem successfully deleted.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR: {0}", ex.Message);
			}
		}
	}
}

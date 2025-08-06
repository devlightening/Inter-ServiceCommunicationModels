HttpClient client = new HttpClient();
HttpResponseMessage message = await client.GetAsync("https://localhost:7155/api/values");
if (message.IsSuccessStatusCode)
{
    string responseBody = await message.Content.ReadAsStringAsync();
    Console.WriteLine(responseBody);
}
else
{
    Console.WriteLine($"Error: {message.StatusCode}");
}

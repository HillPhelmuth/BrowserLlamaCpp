using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrowserLlamaCpp.Shared;

public static class Helpers
{
    public static string GetDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes is { Length: > 0 } ? attributes[0].Description : value.ToString();
    }
    public static string GetModelUrl(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes = (ModelUrlAttribute[])fi.GetCustomAttributes(typeof(ModelUrlAttribute), false);

        return attributes is { Length: > 0 } ? attributes[0].Url : value.ToString();
    }
    public static async Task<double> GetFileSizeFromUrl(string url)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Head, url);
        long size = 0;
        try
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.Contains("Content-Length"))
                {
                    var fileSize = response.Content.Headers.ContentLength ?? 0;
                    size = fileSize;
                    Console.WriteLine($"File size: {fileSize} bytes");
                }
                else
                {
                    Console.WriteLine("Content-Length header is not present.");
                }
            }
            else
            {
                Console.WriteLine($"Failed to retrieve headers. Status code: {response.StatusCode}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return ConvertBytesToMegabytes(size);
    }
    public static double ConvertBytesToMegabytes(long bytes)
    {
        const double bytesInMegabyte = 1024 * 1024;
        return bytes / bytesInMegabyte;
    }
       
}
using OpenAI.Images;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGet("/generateImage", async Task<IResult> (string input) =>
{
    var imageClient = new ImageClient("dall-e-3","<get API key from OpenAI portal>"); //get it from here: https://platform.openai.com/api-keys
    
    var generateOption = new ImageGenerationOptions{
        Quality = GeneratedImageQuality.High,
        Size = GeneratedImageSize.W1024xH1024,
        Style = GeneratedImageStyle.Natural,
        ResponseFormat = GeneratedImageFormat.Uri
    };
    
    var response = await imageClient.GenerateImageAsync(input, generateOption);
    
    return Results.Ok(response.Value.ImageUri);
});
app.MapPost("/editGeneratedImage", async Task<IResult> () =>
{   
    var imageClient = new ImageClient("dall-e-2","<get API key from OpenAI portal>");//get it from here: https://platform.openai.com/api-keys
    
    var imageRequest = new ImageVariationOptions{
        Size = GeneratedImageSize.W1024xH1024,
        ResponseFormat = GeneratedImageFormat.Uri
    };
    
    var response = await imageClient
    .GenerateImageVariationsAsync("./images/art.png",1, imageRequest); // The 2nd paramater can be more than one. The "Value" is an array object
    
    return Results.Ok(response.Value[0].ImageUri);
});
app.Run();
app.MapPost("/GenerateRandomImageBasedOnFile", async Task<IResult> () =>
{   
    var imageClient = new ImageClient("dall-e-2","<get API key from OpenAI portal>");//get it from here: https://platform.openai.com/api-keys
    
    
    return Results.Ok(new {});
});
app.Run();


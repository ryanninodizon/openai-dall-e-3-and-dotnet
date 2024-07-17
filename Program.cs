using OpenAI.Images;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    var imageClient = new ImageClient("dall-e-3","<get API key from OpenAI portal>");
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
    var imageClient = new ImageClient("dall-e-2","<get API key from OpenAI portal>");
    var imageRequest = new ImageVariationOptions{
        Size = GeneratedImageSize.W1024xH1024,
        ResponseFormat = GeneratedImageFormat.Uri
    };
    var response = await imageClient
    .GenerateImageVariationsAsync("./images/art.png",1, imageRequest);
    return Results.Ok(response.Value[0].ImageUri);
});
app.Run();


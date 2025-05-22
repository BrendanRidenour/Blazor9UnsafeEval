using BlazorUnsafeEval.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var policy = new HeaderPolicyCollection();
policy.AddContentSecurityPolicy(csp =>
{
    // This does NOT work:
    csp.AddScriptSrc().Self();

    // This is REQUIRED to make it work:
    //csp.AddScriptSrc().Self().WasmUnsafeEval();
});
app.UseSecurityHeaders(policy);

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorUnsafeEval.Client._Imports).Assembly);

app.Run();
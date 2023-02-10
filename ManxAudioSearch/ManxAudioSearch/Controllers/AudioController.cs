using System.Text.RegularExpressions;
using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManxAudioSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioController : ControllerBase
{
    private readonly AudioService _service;

    public AudioController(AudioService service)
    {
        _service = service;
    }
    
    [HttpGet("{name}")]
    public ActionResult Get(string name)
    {
        if (!_service.ContainsFileNamed(name))
        {
            throw new InvalidOperationException($"{name} not found");
        }
        Regex regex = new Regex(@"([a-zA-Z0-9çÇ\(\)\s_\-:])+$");   
        Match match = regex.Match(name);
        if (!match.Success)
        {
            throw new InvalidOperationException("Possible path traversal");
        }
        
        var path = Path.Combine(AudioService.AudioFilesDir, name + ".mp3");
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
               
        var br = new BinaryReader(fs);
        long numBytes = new FileInfo(path).Length;
        var buff = br.ReadBytes((int)numBytes);
               
        
        var file = File(buff, "audio/mpeg", $"{name}.mp3");
        file.EnableRangeProcessing = true;
        return file;
    }
}
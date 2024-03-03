using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private static string ApiKey = "AIzaSyCnZoMX4eeIAfUcMQPoAa7ceSe3sP1yxl8";
        private static string Bucket = "prn221-anhbang.appspot.com";
        private static string AuthEmail = "damanhbangthpt@gmail.com";
        private static string AuthPassword = "Anhbang12345";

        public FirebaseApp App => throw new NotImplementedException();

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or empty.");

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var cancellation = new CancellationTokenSource();
            var storage = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

            var imageRef = storage
                .Child("receipts")
                .Child("test")
                .Child(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var downloadUrl = await imageRef.PutAsync(stream, cancellation.Token);
                return Ok(downloadUrl);
            }
        }
    }
}

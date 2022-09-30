using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace face_quickstart
{
    class Program
    {
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }
        
    public static async Task DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
    {
        Console.WriteLine("======== DETECÇÃO DE ROSTOS ========");
        Console.WriteLine();				
        IList<DetectedFace> detectedFaces; 		

        detectedFaces = await client.Face.DetectWithUrlAsync($"{url}",  			
                  returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Age	
        ,FaceAttributeType.Emotion, FaceAttributeType.FacialHair, FaceAttributeType.Hair,
        FaceAttributeType.Gender, FaceAttributeType.Glasses,
        FaceAttributeType.Makeup, FaceAttributeType.Smile},
        recognitionModel: recognitionModel);

        Console.WriteLine($" {detectedFaces.Count} rostos detectados na imagem!");		
        Console.WriteLine();
        
        foreach (var face in detectedFaces)
        {
            Console.WriteLine($" ATRIBUTOS DO ROSTO");
            Console.WriteLine($" Retângulo/Posição (Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");
            Console.WriteLine($" Idade : {face.FaceAttributes.Age}");
            Console.WriteLine($" Gênero : {face.FaceAttributes.Gender}");
            Console.WriteLine($" Óculos: {face.FaceAttributes.Glasses}");
            Console.WriteLine($" Maquiagem : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
            Console.WriteLine($" Cabelo facial/barba/bigode : {string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No")}");
            Console.WriteLine($" Sorrindo: {face.FaceAttributes.Smile }");
            Hair hair = face.FaceAttributes.Hair;
            string color = null;
            if (hair.HairColor.Count == 0) { if (hair.Invisible) { color = "Invisible"; } else { color = "Bald"; } }
                HairColorType returnColor = HairColorType.Unknown;
                double maxConfidence = 0.0f;
                foreach (HairColor hairColor in hair.HairColor)
                {
                    if (hairColor.Confidence <= maxConfidence) { continue; }
                    maxConfidence = hairColor.Confidence; returnColor = hairColor.Color; color = returnColor.ToString();
                }
            Console.WriteLine($" Cor do cabelo : {color}");
            Console.WriteLine();
            Console.WriteLine($" EMOÇÕES DO ROSTO");
            Console.WriteLine($" Raiva : " + face.FaceAttributes.Emotion.Anger);
            Console.WriteLine($" Desprezo : " + face.FaceAttributes.Emotion.Contempt);
            Console.WriteLine($" Nojo : " + face.FaceAttributes.Emotion.Disgust);
            Console.WriteLine($" Medo : " + face.FaceAttributes.Emotion.Fear);
            Console.WriteLine($" Felicidade : " + face.FaceAttributes.Emotion.Happiness);
            Console.WriteLine($" Neutro : " + face.FaceAttributes.Emotion.Neutral);
            Console.WriteLine($" Tristeza : " + face.FaceAttributes.Emotion.Sadness);
            Console.WriteLine($" Surpresa : " + face.FaceAttributes.Emotion.Surprise);
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();
        }
            
    }

        static void Main(string[] args)
        {   const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
            const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
            string url_do_video;
            IFaceClient client = Authenticate("https://detectarrosto.cognitiveservices.azure.com/", "e2f914f812154483be98d7d808a0fecd");
            
            Console.Clear();
            url_do_video = "https://imagens.canaltech.com.br/235861.470219-StyleGAN.jpg";
            DetectFaceExtract(client, url_do_video, RECOGNITION_MODEL2).Wait();
            Console.ReadLine();
            
            Console.Clear();
            url_do_video = "https://www.odebate.com.br/wp-content/uploads/2019/01/raiva.jpg";
            DetectFaceExtract(client, url_do_video, RECOGNITION_MODEL2).Wait();
            Console.ReadLine();
            
            Console.Clear();
            url_do_video = "https://media.istockphoto.com/photos/surprised-young-man-portrait-picture-id521680719";
            DetectFaceExtract(client, url_do_video, RECOGNITION_MODEL2).Wait();

        }
    }
}
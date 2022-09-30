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
        Console.WriteLine();				// No exemplo do Tutorial da Microsoft eles realizavam uma análise de um conjunto de fotos,
           						// na minha implementação quero que a API analise uma foto por vez
        IList<DetectedFace> detectedFaces; 		// Declaração de uma lista para separar as faces encontradas na foto

        detectedFaces = await client.Face.DetectWithUrlAsync($"{url}",  			// Requisição da análise da foto à API
                  returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Age	// Selecionei alguns atributos para ela me retornar
        ,FaceAttributeType.Emotion, FaceAttributeType.FacialHair, FaceAttributeType.Hair,
        FaceAttributeType.Gender, FaceAttributeType.Glasses,
        FaceAttributeType.Makeup, FaceAttributeType.Smile},
        recognitionModel: recognitionModel);							// e o modelo de reconhecimento

        Console.WriteLine($" {detectedFaces.Count} rostos detectados na imagem!");		// O .count da nossa lista é um contador de numeros de faces detectadas na foto
        Console.WriteLine();
        // Printa atributos de cada Face
        foreach (var face in detectedFaces)			// O foreach percorre de face em face a nossa lista printando os atributos
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
            //INTERFACE PARA AUTENTICAR O SEU RECURSO DE RECONHECIMENTO FACIAL DO AZURE
	        //IFaceClient client = Authenticate(<ENDPOINT>, <SUBSCRIPTION_KEY>);
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

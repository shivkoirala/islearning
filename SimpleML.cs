using Microsoft.ML;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using System;
using System.Collections.Generic;

namespace ConsoleApp6
{
    class FeedbackData
    {
        [Column(ordinal: "0" , name:"Label" )]
        public bool IsGood { get; set; }
        
        [Column(ordinal: "1" )]
        public string FeedBack { get; set; }
    }
   
    class FeedbackPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsGood { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }

    }
    class Program
    {
        static List<FeedbackData> traindata = new List<FeedbackData>();
        static List<FeedbackData> testData = new List<FeedbackData>();
        static void AddTrainingData( bool rating , string feedback)
        {
            traindata.Add(new FeedbackData()
            {
                IsGood = rating,
                FeedBack = feedback
            });
        }
        static void AddTestData(bool rating, string feedback)
        {
            testData.Add(new FeedbackData()
            {
                IsGood = rating,
                FeedBack = feedback
            });
        }
        static void LoadTestData()
        {
            AddTrainingData(true, "this is so Good");
            AddTrainingData(false, "very bad");
            AddTrainingData(false, "badly man");
            AddTrainingData(true, "oh my god very Good");
            AddTrainingData(true, "it very Average");
            AddTrainingData(false, "bad horrible");
            AddTrainingData(true, "well ok ok");
            AddTrainingData(false, "shitty terrible");
            AddTrainingData(true, "soooo nice");
            AddTrainingData(true, "cool nice");
            AddTrainingData(false, "bad");
            AddTrainingData(false, "bad very badly");
            AddTrainingData(true, "sweet and nice");
            AddTrainingData(true, "nice and Good");
            AddTrainingData(true, "very Good");
            AddTrainingData(true, "quiet Average");
            AddTrainingData(false, "my god horrible");
            AddTrainingData(true, "average and ok");
            AddTrainingData(false, "bad and hell");
            AddTrainingData(true, "nicely done");
            AddTrainingData(true, "this is nice but better can be done");
            AddTrainingData(true, "till now it looks nice");
            AddTrainingData(true, "Good and bravo");
            AddTrainingData(true, "very Good");
            AddTrainingData(true, "Average and ok ok ");
            AddTrainingData(false, "bad bad bad bad bad bad");

            AddTestData(true, "good");
            AddTestData(true, "nice");
            AddTestData(true, "ok");
            AddTestData(false, "terrible");
            AddTestData(false, "horrible");
            AddTestData(false, "bad");

        }
        static void Main(string[] args)
        {
            // Training data
            LoadTestData();
            MLContext mlContext = new MLContext(seed: 0);
            IDataView dataView =mlContext.
                                CreateStreamingDataView<FeedbackData>(traindata);
            // Train the alogorithm
            var pipeline = mlContext.Transforms.Text.FeaturizeText("FeedBack","Features")
                            .Append(mlContext.BinaryClassification.Trainers.FastTree
                                (numLeaves: 50, numTrees: 50, minDatapointsInLeaves: 1));
            var model = pipeline.Fit(dataView);

            // Test the model
            IDataView dataView1 = mlContext.
                                CreateStreamingDataView<FeedbackData>(testData);

            var predictions = model.Transform(dataView1);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");

            Console.WriteLine();
            Console.WriteLine("Model quality");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
           
            Console.WriteLine("Model evaluation done");
            Console.Read();
            // Use the model
            var predictionFunction = model.
                                        MakePredictionFunction
                                        <FeedbackData,FeedbackPrediction>(mlContext);
            while (1 == 1)
            {
                FeedbackData o = new FeedbackData();
                o.FeedBack = Console.ReadLine().Trim();
                var resultprediction = predictionFunction.Predict(o);
                Console.WriteLine(resultprediction.IsGood);
                Console.WriteLine(resultprediction.Probability);
                Console.WriteLine(resultprediction.Score);
                Console.Read();
            }
           

        }
    }
}

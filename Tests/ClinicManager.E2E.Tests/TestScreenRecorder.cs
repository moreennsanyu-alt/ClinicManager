using System;
using System.IO;
using System.Threading;
using ScreenRecorderLib;

namespace ClinicManager.E2E.Tests
{
    public class TestScreenRecorder
    {
        private static readonly TimeSpan StopTimeout = TimeSpan.FromSeconds(15);

        public Recorder _rec;
        private readonly string _recordingsDirectory;
        private readonly string _testRecordingFile;
        private readonly ManualResetEventSlim _finished = new(false);

        public TestScreenRecorder(string recordingsDirectory, string testRecordingFile)
        {
            _recordingsDirectory = recordingsDirectory;
            _testRecordingFile = testRecordingFile;
        }

        public void CreateRecording()
        {
            Directory.CreateDirectory(_recordingsDirectory);
            string videoPath = Path.Combine(_recordingsDirectory, _testRecordingFile);
            File.Delete(videoPath);

            _rec = Recorder.CreateRecorder();
            _rec.OnRecordingComplete += Rec_OnRecordingComplete;
            _rec.OnRecordingFailed += Rec_OnRecordingFailed;
            _rec.OnStatusChanged += Rec_OnStatusChanged;
            _rec.Record(videoPath);
        }

        public void EndRecording()
        {
            _rec.Stop();
            // Stop() is asynchronous: wait until the file is finalized
            _finished.Wait(StopTimeout);
            _rec.Dispose();
        }

        private void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            //Get the file path if recorded to a file
            string path = e.FilePath;
            _finished.Set();
        }

        private void Rec_OnRecordingFailed(object sender, RecordingFailedEventArgs e)
        {
            string error = e.Error;
            Console.WriteLine(error);
            _finished.Set();
        }

        private void Rec_OnStatusChanged(object sender, RecordingStatusEventArgs e)
        {
            // Handle status changes if needed
        
}
    }
}

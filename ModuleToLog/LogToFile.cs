using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModuleToLog
{
    public class LogToFile
    {
        private Semaphore _fileSemaphore;
        private string _path;
        private List<string> _buffer;
        private int _bufferSize;
        private bool _forceBufferDump;                          //mozna zrobic go publicznym do zewnetrznego wymuszania
                                                                //oproznienia bufora

        public LogToFile(string path, int sizeOfBuffer)         //W konstruktorze tworzenie semafora, ustalenie
        {                                                       //sciezki dostepu do pliku oraz rozmiaru bufory dla
            _fileSemaphore = new Semaphore(1, 1);               //zapisywanych komunikatow.
            _path = path;
            _buffer = new List<string>();
            _bufferSize = sizeOfBuffer;
            _forceBufferDump = false;
        }

        public LogToFile(string path) : this(path, 20)
        {
        }

        ~LogToFile()                                                //Zapisanie informacji pozostalych w buforze
        {                                                           //na zakonczenie istnienia obiektu.
            _forceBufferDump = true;
            TextToFile("Koniec zycia obiektu");
        }

        public void TextToFile(string text)
        {
            try
            {
                _fileSemaphore.WaitOne();
                _buffer.Add(AddDate(text));
                if (_buffer.Count >= _bufferSize || _forceBufferDump)
                {
                    toFile();
                    _forceBufferDump = false;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        private void toFile()
        {
            try
            {
                System.IO.File.AppendAllLines(_path, _buffer);
                _buffer.Clear();
            }
            catch (Exception e)                                     //Prosta obsluga wyjatku, mozliwa rozbudowa
            {
                Console.WriteLine(e);
            }
        }

        private string AddDate(string text)
        {
            DateTime timestamp = DateTime.Now;
            text = timestamp.ToString() + " " + text;
            return text;
        }
    }
}

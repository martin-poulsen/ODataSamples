﻿using System;
using Microsoft.OData.Core;
using ODataSamples.Common;
using ODataSamples.Common.Model;
using ODataSamples.Common.Payload;

namespace ODataSamples.Reader
{
    class Program
    {
        private static readonly ParserExtModel ExtModel = new ParserExtModel();

        static void Main(string[] args)
        {
            ReadEntry(true);
            ReadEntry(false);
        }

        // Simple demo for reading entry
        public static void ReadEntry(bool enableFullValidation)
        {
            var message = new Message()
            {
                Stream = Payloads.GetStreamFromResource("ODataSamples.Common.Payload.Entry.txt")
            };

            ODataEntry entry = null;

            var setting = new ODataMessageReaderSettings()
            {
                EnableFullValidation = enableFullValidation
            };

            using (var messageReader = new ODataMessageReader((IODataResponseMessage)message, setting, ExtModel.Model))
            {
                var reader = messageReader.CreateODataEntryReader(ExtModel.Person);
                while (reader.Read())
                {
                    switch (reader.State)
                    {
                        case ODataReaderState.EntryEnd:
                            entry = (ODataEntry)reader.Item;
                            break;
                    }
                }
            }

            Console.WriteLine(entry.Id);
            foreach (var property in entry.Properties)
            {
                Console.WriteLine("{0}:{1}", property.Name, property.Value);
            }
        }
    }
}

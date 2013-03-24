﻿using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Reports
{
    public class TestData
    {
        private static int NUMBER_OF_REQUESTS = 1000;
        private static int NUMBER_OF_QUESTIONS = 2 * NUMBER_OF_REQUESTS;
        private static DateTime START_DATETIME = new DateTime(2011, 1, 1);
        private static DateTime END_DATETIME = new DateTime(2013, 12, 31);
        private static DREAMContext db = new DREAMContext();

        private static Random rand = new Random();

        public static List<Question> Questions = null;
        public static List<Request> Requests = null;

        public static void Initialize()
        {
            while(Membership.GetAllUsers().Count < 6)
            {
                string username = generateRandomString(12);
                Membership.CreateUser(username, Membership.GeneratePassword(32, 5), username + "@example.com");
            }

            while (db.Keywords.Count() < 20)
            {
                string keyword = generateRandomString(20);
                db.Keywords.Add(new Keyword
                {
                    Enabled = true,
                    KeywordText = keyword,
                });
                db.SaveChanges();
            }

            Requests = generateRandomRequests();
            Questions = generateRandomQuestions(Requests);
        }

        private static List<Question> generateRandomQuestions(List<Request> requests)
        {
            List<Question> questions = new List<Question>();
            for (int i = 0; i < NUMBER_OF_QUESTIONS; i++)
            {
                List<Keyword> keywords = new List<Keyword>();
                int numKeywords = rand.Next(2) + 1;
                for (int j = 0; j < numKeywords; j++)
                {
                    keywords.Add(pickRandom(db.Keywords));
                }

                List<Reference> references = new List<Reference>();
                int numReferences = rand.Next(2) + 1;
                for (int j = 0; j < numReferences; j++)
                {
                    references.Add(new Reference
                    {
                        QuestionID = i,
                        ReferenceType = ReferenceType.TEXT,
                        Value = generateRandomString(20),
                    });
                }

                Request request = pickRandom(requests);

                Question newQuestion = new Question
                {
                    ID = i,
                    TumourGroup = pickRandom(db.TumourGroups),
                    TimeTaken = rand.Next(240),
                    SpecialNotes = "",
                    Severity = rand.Next(5),
                    Response = "",
                    Request = request,
                    Reference = references,
                    QuestionType = pickRandom(db.QuestionTypes),
                    QuestionText = "",
                    Probability = rand.Next(5),
                    Keywords = keywords,
                };

                questions.Add(newQuestion);
                request.Questions.Add(newQuestion);
            }

            return questions;
        }

        private static List<Request> generateRandomRequests()
        {
            List<Request> requests = new List<Request>();

            IEnumerable<MembershipUser> users = Membership.GetAllUsers().Cast<MembershipUser>();

            for (int i = 0; i < NUMBER_OF_REQUESTS; i++)
            {
                DateTime startDate = generateRandomDate();
                requests.Add(new Request
                {
                    ID = i,
                    CreationTime = startDate,
                    CompletionTime = generateRandomDate(startDate),
                    CreatedBy = (Guid)pickRandom(users).ProviderUserKey,
                    ClosedBy = (Guid)pickRandom(users).ProviderUserKey,
                    Caller = new Caller
                    {
                        ID = i,
                        FirstName = generateRandomString(20),
                        LastName = generateRandomString(20),
                        Email = generateRandomString(20) + "@example.com",
                        Region = pickRandom(db.Regions),
                        RequestID = i,
                        PhoneNumber = "(555) 555-5555",
                        Type = pickRandom(db.RequesterTypes),
                    },
                    Patient = new Patient
                    {
                        Age = rand.Next(100),
                        AgencyID = rand.Next(100000),
                        FirstName = generateRandomString(20),
                        Gender = (int)Gender.UNKNOWN,
                        ID = i,
                        LastName = generateRandomString(20),
                    },
                });
            }

            return requests;
        }

        private static string generateRandomString(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        private static DateTime generateRandomDate(DateTime? startDate=null)
        {
            int dayRange = (END_DATETIME - START_DATETIME).Days;

            if(startDate != null)
            {
                dayRange = 10;
            }
            else
            {
                startDate = START_DATETIME;
            }

            return ((DateTime)startDate).AddDays(dayRange * rand.NextDouble());
        }

        private static T pickRandom<T>(IEnumerable<T> objects)
        {
            int choice = (int)(rand.NextDouble() * objects.Count());
            return objects.ToArray()[choice];
        }
    }
}
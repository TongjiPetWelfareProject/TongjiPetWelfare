using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.Model
{
    public class Pet2
    {
        public struct Comment
        {
            public string comment_contents;
            public DateTime comment_time;
            public Comment(string co, DateTime comdt)
            {
                comment_contents = co;
                comment_time = comdt;
            }
        }
        public Pet2()
        {

        }
        public Pet original_pet;
        public int Popularity;
        public bool sex;
        public string Psize;//宠物大小
        public int Comment_Num;
        public Comment[] comments;
    }
}

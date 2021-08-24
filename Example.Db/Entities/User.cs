using Example.Db.Base;
using System;

namespace Example.Db.Entities
{
    public class User : BaseModel
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// </summary>
        public DateTime CreateDate { get; set; }
     


    }
}

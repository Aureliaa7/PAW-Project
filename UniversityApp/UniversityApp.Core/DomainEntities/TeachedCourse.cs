using System;

namespace UniversityApp.Core.DomainEntities
{
    public class TeachedCourse
    {
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }

        public Guid CourseId { get; set; }


        public virtual Course Course { get; set; }
       
        public virtual Teacher Teacher { get; set; }
    }
}

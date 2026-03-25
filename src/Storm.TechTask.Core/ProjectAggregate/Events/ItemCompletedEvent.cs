using System;
using System.Collections.Generic;
using System.Text;

using Storm.TechTask.SharedKernel.Entities; // need this as BaseDomainEvent is referenced whcih is in SharedKernel.Entities

namespace Storm.TechTask.Core.ProjectAggregate.Events
{
    public class ItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem Item { get; set; }
        public Project Project { get; set; }

        public ItemCompletedEvent(Project project,
            ToDoItem item)
        {
            Project = project;
            Item = item;
        }
    }
}
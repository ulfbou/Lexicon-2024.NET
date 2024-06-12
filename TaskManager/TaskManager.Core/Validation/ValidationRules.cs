using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Validation
{
    public class ValidationRules
    {
        public bool ValidateDueDate(DateTime dueDate)
        {
            return dueDate >= DateTime.Now;
        }

        public bool ValidatePriority(int priority)
        {
            return priority >= 1 && priority <= 5;
        }

        // Additional validation rules can be added here
    }
}

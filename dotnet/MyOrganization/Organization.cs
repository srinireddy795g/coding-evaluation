using System.Text;

namespace MyOrganization
{
    internal abstract class Organization
    {
        private Position root;
        // Initialized empId to 0 and is incremented in the method to add as Employee Identifier
        int empId = 0;

        public Organization()
        {
            root = CreateOrganization();
        }

        protected abstract Position CreateOrganization();

        /**
         * hire the given person as an employee in the position that has that title
         * 
         * @param person
         * @param title
         * @return the newly filled position or empty if no position has that title
         */
        /// <summary>
        ///  Hire method to check and fill position if not already filled
        /// </summary>   
        /// <param name="person"></param>
        /// <param name="title"></param>
        /// <returns>Position?</returns>
        public Position? Hire(Name person, string title)
        {
            //your code here           

            var isRootTitle = root?.GetTitle() == title;

            if (isRootTitle)
            {
                return FillPosition(person, root);
            }
            else
            {
                return CheckTitleAndFillPosition(root, person, title);
            }
        }

        /// <summary>
        ///  Checks if title exists and then fills the position. If not returns null for Position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="person"></param>
        /// <param name="title"></param>
        /// <returns>Returns Position if title exists and filled if not null</returns>
        private Position? CheckTitleAndFillPosition(Position position, Name person, string title)
        {
            if (position == null)
            {
                return null;
            }

            if (position.GetTitle() == title)
            {
                return FillPosition(person, position);
            }
            else
            {
                foreach (var directReport in position.GetDirectReports())
                {
                    // Recursive method called is used to check for each position if any directreports exists
                    var result = CheckTitleAndFillPosition(directReport, person, title);
                    if (result != null) return result;
                }

                return null;
            }
        }

        /// <summary>
        ///  FillPosition to fill position if not already filled. If filled outputs the filled employee details
        /// </summary>
        /// <param name="person"></param>
        /// <param name="position"></param>
        /// <returns>Returns Position if not filled or filled</returns>
        private Position? FillPosition(Name person, Position position)
        {
            if (position == null)
            {
                return null;
            }

            if (!position.IsFilled())
            {
                position.SetEmployee(new Employee(++empId, person));
                return position;
            }
            else
            {
                Console.WriteLine($"Position \"{position.GetTitle()}\" already filled with employee \"{position.GetEmployee()}\"");
                return position;
            }
        }

        override public string ToString()
        {
            return PrintOrganization(root, "");
        }

        private string PrintOrganization(Position pos, string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix + "+-" + pos.ToString() + "\n");
            foreach (Position p in pos.GetDirectReports())
            {
                sb.Append(PrintOrganization(p, prefix + "  "));
            }
            return sb.ToString();
        }
    }
}

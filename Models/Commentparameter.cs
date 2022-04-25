namespace Twitter.Models;
    public record CommentParameters
    {
        const int maxPageSize = 10;

        public int PageNumber { get; set; }

        public int _pageSize = 10;
         public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize:value;
        }
    }
    }
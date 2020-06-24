using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IRepository<Blog, int> _blogRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<BlogTag, int> _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogService(IRepository<Blog, int> blogRepository,
            IRepository<BlogTag, int> blogTagRepository,
            IRepository<Tag, string> tagRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _blogTagRepository = blogTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BlogViewModel Add(BlogViewModel blogVm)
        {
            var blog = _mapper.Map<BlogViewModel, Blog>(blogVm);

            if (!string.IsNullOrEmpty(blog.Tags))
            {
                var tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.BlogTag
                        };
                        _tagRepository.Add(tag);
                    }

                    var blogTag = new BlogTag { TagId = tagId };
                    blog.BlogTags.Add(blogTag);
                }
            }
            _blogRepository.Add(blog);
            return blogVm;
        }

        public void Delete(int id)
        {
            _blogRepository.Remove(id);
        }

        public List<BlogViewModel> GetAll()
        {
            return _mapper.ProjectTo<BlogViewModel>(_blogRepository.FindAll(c => c.BlogTags))
                .ToList();
        }

        public PagedResult<BlogViewModel> GetAllPaging(string keyword, int pageSize, int page = 1)
        {
            var query = _blogRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<BlogViewModel>()
            {
                Results = _mapper.ProjectTo<BlogViewModel>(data).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }

        public BlogViewModel GetById(int id)
        {
            return _mapper.Map<Blog, BlogViewModel>(_blogRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BlogViewModel blog)
        {
            _blogRepository.Update(_mapper.Map<BlogViewModel, Blog>(blog));
            if (!string.IsNullOrEmpty(blog.Tags))
            {
                string[] tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    _blogTagRepository.RemoveMultiple(_blogTagRepository.FindAll(x => x.Id == blog.Id).ToList());
                    BlogTag blogTag = new BlogTag
                    {
                        BlogId = blog.Id,
                        TagId = tagId
                    };
                    _blogTagRepository.Add(blogTag);
                }
            }
        }

        public List<BlogViewModel> GetLastest(int top)
        {
            return _mapper.ProjectTo<BlogViewModel>(_blogRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public List<BlogViewModel> GetHotProduct(int top)
        {
            return _mapper.ProjectTo<BlogViewModel>(_blogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top))
                .ToList();
        }

        public List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return _mapper.ProjectTo<BlogViewModel>(query.Skip((page - 1) * pageSize)
                .Take(pageSize))
                .ToList();
        }

        public List<string> GetListByName(string name)
        {
            return _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(name)).Select(y => y.Name).ToList();
        }

        public List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return _mapper.ProjectTo<BlogViewModel>(query.Skip((page - 1) * pageSize)
                .Take(pageSize))
                .ToList();
        }

        public List<BlogViewModel> GetReatedBlogs(int id, int top)
        {
            return _mapper.ProjectTo<BlogViewModel>(_blogRepository.FindAll(x => x.Status == Status.Active
                && x.Id != id)
            .OrderByDescending(x => x.DateCreated)
            .Take(top))
            .ToList();
        }

        public List<TagViewModel> GetListTagById(int id)
        {
            return _mapper.ProjectTo<TagViewModel>(_blogTagRepository.FindAll(x => x.BlogId == id, c => c.Tag)
                .Select(y => y.Tag))
                .ToList();
        }

        public void IncreaseView(int id)
        {
            var product = _blogRepository.FindById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public List<BlogViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _blogRepository.FindAll()
                        join pt in _blogTagRepository.FindAll()
                        on p.Id equals pt.BlogId
                        where pt.TagId == tagId && p.Status == Status.Active
                        orderby p.DateCreated descending
                        select p;

            totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var model = _mapper.ProjectTo<BlogViewModel>(query);
            return model.ToList();
        }

        public TagViewModel GetTag(string tagId)
        {
            return _mapper.Map<Tag, TagViewModel>(_tagRepository.FindSingle(x => x.Id == tagId));
        }

        public List<BlogViewModel> GetList(string keyword)
        {
            var query = !string.IsNullOrEmpty(keyword) ?
                _blogRepository.FindAll(x => x.Name.Contains(keyword))
                : _blogRepository.FindAll();
            return _mapper.ProjectTo<BlogViewModel>(query).ToList();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            return _mapper.ProjectTo<TagViewModel>(_tagRepository.FindAll(x => x.Type == CommonConstants.ProductTag
            && searchText.Contains(x.Name))).ToList();
        }
    }
}
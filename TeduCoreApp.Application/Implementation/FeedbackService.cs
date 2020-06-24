using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementation
{
    public class FeedbackService : IFeedbackService
    {
        private IRepository<Feedback, int> _feedbackRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IRepository<Feedback, int> feedbackRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(FeedbackViewModel feedbackVm)
        {
            var page = _mapper.Map<FeedbackViewModel, Feedback>(feedbackVm);
            _feedbackRepository.Add(page);
        }

        public void Delete(int id)
        {
            _feedbackRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<FeedbackViewModel> GetAll()
        {
            return _mapper.ProjectTo<FeedbackViewModel>(_feedbackRepository.FindAll()).ToList();
        }

        public PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _feedbackRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<FeedbackViewModel>()
            {
                Results = _mapper.ProjectTo<FeedbackViewModel>(data).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public FeedbackViewModel GetById(int id)
        {
            return _mapper.Map<Feedback, FeedbackViewModel>(_feedbackRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(FeedbackViewModel feedbackVm)
        {
            var page = _mapper.Map<FeedbackViewModel, Feedback>(feedbackVm);
            _feedbackRepository.Update(page);
        }
    }
}
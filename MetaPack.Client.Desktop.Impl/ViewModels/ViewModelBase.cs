using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class TypedViewModelBase<TClass, TDtoObject> : ViewModelBase<TClass>
        where TDtoObject : class, new()
    {
        public override string ToString()
        {
            if (Dto != null)
                return Dto.ToString();

            return base.ToString();
        }

        public TypedViewModelBase()
            : this(new TDtoObject())
        {

        }

        public TypedViewModelBase(TDtoObject dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            this.Dto = dto;
        }

        public TDtoObject Dto { get; set; }

        public static IEnumerable<TModelView> FromDto<TModelView>(IEnumerable<TDtoObject> dtos)
            where TModelView : TypedViewModelBase<TClass, TDtoObject>, new()
        {
            var result = new List<TModelView>();

            foreach (var dto in dtos)
            {
                var viewModel = new TModelView();
                viewModel.Dto = dto;

                result.Add(viewModel);
            }

            return result;
        }

        public static IEnumerable<TDtoObject> ToDto<TViewModel>(IEnumerable<TViewModel> viewModels)
            where TViewModel : TypedViewModelBase<TClass, TDtoObject>
        {
            return viewModels.Select(s => s.Dto);
        }
    }

    public static class ViewModelUtils
    {

    }


    public class ViewModelBase<TClass> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged
            = delegate { };

        protected virtual void OnPropertyChanged<TValue>(Expression<Func<TClass, TValue>> propertySelector)
        {
            if (PropertyChanged == null)
                return;

            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }
    };
}
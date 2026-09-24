using System.Windows;
using View.Data;

namespace View.Modules.PlatformModule.Sections
{
    public abstract class SectionBase
    {
        protected PlatformMain Host { get; }
        protected IDbWrapper Db => Host.Db;
        protected int RoleId => Host.RoleId;

        protected SectionBase(PlatformMain host) => Host = host;

        public abstract string Title { get; }

        public abstract bool IsVisible { get; }

        public virtual bool CanEdit => RoleId == 1;

        public virtual bool CanDelete => RoleId == 1;

        public virtual bool CanCreate => RoleId == 1;

        public abstract FrameworkElement Build();
    }
}
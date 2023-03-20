namespace Infra
{
    public enum CheckIsAdminState
    {
        IsAdmin = 1,
        IsNotAdmin,
        IsErrorCreateToken
    }

    public enum UserTypeState
    {
        SuperAdmin = 1,
        AdminCenter,
        AdminBranch,
        Employee,
        ExcelReport,
        EmployeeCenter,
    }

    public enum BaseAccountType
    {
        Individual = 1,
        Companies,
        Certified
    }

    public enum OrderItemState
    {
        Success = AccountState.IsActive,
        IsSuspended = AccountState.IsSuspended,
    }

    public enum AccountState
    {
        IsActive = 1,
        IsSuspended
    }

    public enum InputTypeState
    {
        Defualt = 1,// ادخال من الواجهة
        ImportExcel,
        API,
        FlixCupe,
        Cosher
    }

    public enum InputAccountTypeState
    {
        Defualt = 1,// ادخال من الواجهة
        ImportExcel,
        API,
    }

    public enum WeekDayState
    {
        Saturday = 1,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
    }

    public enum OrderRequestState
    {
        Process = 1,
        RejectRequest,
        OrderRequestPrinting,
        OrderRequestPrintedDone,
        IsRejectedByCenter,
        IsFrozen,
        Pinding,
        GeneretedCode,
        SendRequestBranch,
        SendCenter,
        PrintOutCenter,
    }

    public enum EventTypeState
    {
        Insert = 1,
        Delete,
        Update,
        Activation,
        Login,
        OrderRequest,
        Exception,
        ApprovedOrderRequest,
        RejectOrderRequest,
        FreezeOrderItem
    }









}

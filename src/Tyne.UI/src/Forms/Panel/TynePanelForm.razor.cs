namespace Tyne.UI.Forms;

public abstract partial class TynePanelForm<TOpen, TModel> : TyneModalForm<TOpen, TModel> where TModel : class
{
	protected virtual string Width => "420px";
}

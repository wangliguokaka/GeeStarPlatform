
function LogOutUrl(confirmText) {
    /// <summary>
    ///     转到注销页面
    /// </summary>
    /// <param name="confirmText" type="String">
    ///     询问文本
    /// </param>

    if (confirm(confirmText) == false) {
        return;
    }

    top.location = 'LogOutPage.aspx';

    return false;
}

function Refresh(cultureType,confirmText, errorText) {
    /// <summary>
    ///     重新加载页面
    /// </summary>
    /// <param name="confirmText" type="String">
    ///     询问文本
    /// </param>
    /// <param name="errorText" type="String">
    ///     错误文本
    /// </param>

    if (confirm(confirmText) == false) {
        return false
    }
    else{
        return true;
    }

    try {
        //if (cultureType == 'zhCN') {
        //    $("#LbChinese").hide();
        //    $("#LbEnglish").show();
        //}
        //else {
        //    $("#LbChinese").show();
        //    $("#LbEnglish").hide();
        //}
    }
    catch (ex) {
        alert(errorText);
    }
}
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AttachmentFile.aspx.cs" Inherits="OrderManagement_AttachmentFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var jcrop_api;
        function doShareFile() {
                var imgExts = '|.bmp|.jpg|.jpeg|'  //图片文件的后缀名（注意：前后都要加“|”）
                
                if ($("#txtshareF").val() == "") {
                    alertdialog('<%= GetGlobalResourceObject("Resource","UploadPictureAlert").ToString() %>');
                    return false;
                }
                var fileVal = $("#txtshareF").val();
                var ext = fileVal.slice(fileVal.lastIndexOf('.') - fileVal.length);
                ext = '|' + ext.toLowerCase() + '|'
                if (imgExts.indexOf(ext) === -1) {
                    alert('<%= GetGlobalResourceObject("Resource","UploadPictureFormat").ToString() %>');
                    return;
                }


                $('#form2').ajaxSubmit({
                    type: 'post',
                    success: function (data) {
                        if (typeof (jcrop_api) == "undefined") {

                        } else {
                            jcrop_api.destroy();
                        }
                       
                        var values = data.toString().split(',');
                        if (values.length == 3) {
                            $("#viewpic").attr("src", values[0]);
                            $("#viewpic").attr("width", values[1]);
                            $("#viewpic").attr("height", values[2]);
                            $("#picfile").val(values[0]);

                            //                        jQuery('#viewpic').Jcrop({
                            //                            onSelect: showCoords

                            //                        var option = {
                            //                            boxWidth: values[1]+"px",
                            //                            boxHeight: values[2]+"px"
                            //                        }
                            //                        });
                            jcrop_api = $.Jcrop('#viewpic');



                            jQuery('#viewpic').Jcrop({
                                onSelect: showCoords,
                                onChange: showCoords
                            });
                        }
                        else {
                            alertdialog('<%= GetGlobalResourceObject("Resource","UploadAgain").ToString() %>');
                        }
                        art.dialog.list['PIC004'].close();
                    },
                    error: function (data) {
                        alertdialog('<%= GetGlobalResourceObject("Resource","UploadFail").ToString() %>');
                    },
                    cache: false
                });
        }
    </script>

</head>
<body>
    <form id="form2" runat="server" enctype="multipart/form-data">
    <input id="picID" name="picID" type="hidden" value="<%=picID %>" />
    <input id="uploadFile" name="uploadFile" type="hidden" />
    <table>
        <tr style="display:none;">
            <td class="vtdleft">
                <%= GetGlobalResourceObject("Resource","FileName").ToString() %>
            </td>
            <td>
                <input id="txtFileName" name="txtFileName" type="text" />
            </td>
        </tr>
        <tr>
            <td class="vtdleft">
                <%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %>
            </td>
            <td>
                <input id="txtshareF" name="txtshareF" accept=".jpg,.jpeg,.bmp" type="file" />
            </td>
        </tr>
        <tr>
            <td>
                
            </td>
            <td style=" text-align:left;">
                <a href="javascript:void(0)" onclick="doShareFile();" class="btn01"><%= GetGlobalResourceObject("Resource","OK").ToString() %></a>&nbsp;&nbsp;<a href="javascript:void(0)" onclick="dialog.close();" class="btn01"><%= GetGlobalResourceObject("Resource","Close").ToString() %></a>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<%@ Page Title="" Language="C#" MasterPageFile="BackgroundManagement.Master" AutoEventWireup="true" CodeBehind="TableList.aspx.cs" Inherits="Dian.Web.TableList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="admin-content">

        <div class="am-cf am-padding">
            <div class="am-fl am-cf">
                <strong class="am-text-primary am-text-lg">餐桌管理</strong> / <small>
                    <label>列表</label></small>
            </div>
        </div>

        <div class="am-g">
            <div class="am-u-sm-12 am-u-md-6">
                <div class="am-btn-toolbar">
                    <div class="am-btn-group am-btn-group-xs">
                        <button type="button" class="am-btn am-btn-default" onclick="beforeAdd();"><span class="am-icon-plus"></span>新增</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="am-g">
            <div class="am-u-sm-12">
                <div class="am-form">
                    <table class="am-table am-table-striped am-table-hover table-main">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>桌名</th>
                                <th class="restaurant-name">店名</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeater1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("TABLE_ID") %></td>
                                        <td><a href='TableDetail.aspx?op=edit&id=<%# Eval("TABLE_ID") %>'><%# Eval("TABLE_NAME") %></a></td>
                                        <td class="restaurant-name"><%# Eval("RESTAURANT_NAME") %></td>
                                        <td>
                                            <div class="am-btn-toolbar">
                                                <div class="am-btn-group am-btn-group-xs">
                                                    <button type="button" class="am-btn am-btn-default am-btn-xs am-text-secondary am-hide-sm-only" onclick="qrcode('<%# Eval("TABLE_ID") %>');">生成二维码</button>
                                                    <button type="button" class="am-btn am-btn-default am-btn-xs am-text-secondary am-hide-sm-only" onclick="beforeEdit('<%# Eval("TABLE_ID") %>');"><span class="am-icon-pencil-square-o"></span>编辑</button>
                                                    <button type="button" class="am-btn am-btn-default am-btn-xs am-text-danger" onclick="beforeDelete('<%# Eval("TABLE_ID") %>');"><span class="am-icon-trash-o"></span>删除</button>                                                    
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <div class="am-cf">
                        <label id="labelPage"></label>
                        <div class="am-fr" id="divPage">
                            <ul class="am-pagination">
                                <li><a id="aFirst" href="#">第一页</a></li>
                                <li><a id="aPrev" href="#">上一页</a></li>
                                <li><a id="aCur" href="#">1/3</a></li>
                                <li><a id="aNext" href="#">下一页</a></li>
                                <li><a id="aLast" href="#">最末页</a></li>
                            </ul>
                        </div>
                    </div>

                    <label id="lMsg" runat="server" class="am-text-warning"></label>

                </div>
            </div>
        </div>

    </div>

    <br />

    <input type="hidden" id="hDeleteId" runat="server" />
    <input type="hidden" id="hOperation" runat="server" />

    <script type="text/javascript">

        $(function () {
            initPagination('<%= CurPage %>', '<%= TotalCount %>', '<%= PageCount %>', '<%= this.Request.Url.AbsolutePath %>');
            if ('<%= base.CurEmployeeEntity.IS_ADMIN %>' === 'False') $('.restaurant-name').hide();
        });

        function beforeAdd() {
            self.location.href = 'TableDetail.aspx?op=add';
        }

        function beforeEdit(id) {
            self.location.href = 'TableDetail.aspx?op=edit&id=' + id;
        }

        function beforeDelete(id) {
            if (confirm("您确定要删除本条记录吗？\r\n删除点击“确定”，不删除点击“取消”。")) {
                $('#<%= this.hOperation.ClientID %>').val('delete');
                $('#<%= this.hDeleteId.ClientID %>').val(id);
                $('#' + form1).submit();
            }
        }

        function qrcode(id) {
            $('#<%= this.hOperation.ClientID %>').val('qrcode');
            $('#<%= this.hDeleteId.ClientID %>').val(id);
            $('#' + form1).submit();
        }

    </script>

</asp:Content>

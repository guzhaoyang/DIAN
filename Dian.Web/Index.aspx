﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TheFront.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Dian.Web.Index" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="CSS/index.css" rel="stylesheet" />
    <script src="Scripts/index.js"></script>

    <div class="am-cf admin-main" style="background: #fff;">

        <!-- sidebar start -->
        <div id="admin-offcanvas">
            <div id="admin-offcanvas-bar">
                <ul class="am-list admin-sidebar-list">
                    <li><a href="javascript:loadFoodByType('0');">全部</a></li>
                    <asp:Repeater ID="repeater1" runat="server">
                        <ItemTemplate>
                            <li><a href="javascript:loadFoodByType('<%# Eval("FOOD_TYPE_ID") %>');">
                                <%# Eval("FOOD_TYPE_NAME") %>
                                <label id="<%# "lFoodTypeCount" + Eval("FOOD_TYPE_ID").ToString() %>" class="am-badge am-badge-warning am-margin-right"></label>
                            </a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <!-- sidebar end -->

        <!-- content start -->
        <div id="divMenu">
            <div class="admin-content">
                <div data-am-widget="list_news" class="am-list-news am-list-news-default am-no-layout">
                    <div class="am-list-news-bd">
                        <ul class="am-list am-avg-sm-1 am-avg-md-1 am-thumbnails" id="uiFootList">
                            <!--缩略图在标题左边-->

                            <asp:Repeater ID="repeater2" runat="server">
                                <ItemTemplate>
                                    <li class="am-g am-list-item-desced am-list-item-thumbed am-list-item-thumb-left" id="<%# "liFood" + Eval("FOOD_ID").ToString() %>" foodid="<%# Eval("FOOD_ID") %>" foodtypeid="<%# Eval("FOOD_TYPE_ID") %>" foodname="<%# Eval("FOOD_NAME") %>" foodimage="<%# Eval("FOOD_IMAGE1") %>" foodprice="<%# Eval("PRICE") %>">
                                        <div class="am-u-sm-4 am-list-thumb">
                                            <a data-am-modal="{target: '#divRemark', width: 260, height: 420}" href="javascript:loadRemark('<%# Eval("FOOD_ID") %>')">
                                                <img src="<%# Eval("FOOD_IMAGE_NAIL1") %>" alt="<%# Eval("FOOD_NAME") %>" class="am-img-thumbnail">
                                            </a>
                                            &nbsp;
                                        </div>
                                        <div class="am-u-sm-8 am-list-main">
                                            <h3 class="am-list-item-hd">
                                                <a data-am-modal="{target: '#divRemark', width: 260, height: 420}" href="javascript:loadRemark('<%# Eval("FOOD_ID") %>')"><%# Eval("FOOD_NAME") %></a>
                                            </h3>
                                            <div class="am-list-item-text"><%# Eval("DESCRIPTION") %></div>
                                            <div style="margin-top: 10px;">
                                                <label class="am-text-danger">￥<%# Eval("PRICE") %></label>
                                                <span class="am-fr">
                                                    <button type="button" class="am-btn am-btn-success am-btn-xs" onclick="cutFood('<%# Eval("FOOD_ID") %>');">减</button>
                                                    <label id="<%# "lFoodCount" + Eval("FOOD_ID").ToString() %>">0</label>
                                                    <button type="button" class="am-btn am-btn-success am-btn-xs" onclick="addFood('<%# Eval("FOOD_ID") %>');">加</button>
                                                </span>
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>

                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="divCart" style="display: none;">

            <div class="admin-content">

                <div class="am-g">
                    <div id="divConfirmCart" class="am-u-sm-12">
                        <div class="am-list-item-text">已确认的菜单</div>
                        <table id="tConfirmCart" class="am-table am-table-bd am-table-striped admin-content-table">
                            <thead>
                                <tr>
                                    <th>菜名</th>
                                    <th>单价</th>
                                    <th>份数</th>
                                    <th>状态</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <hr />
                    </div>
                    <div class="am-u-sm-12">
                        <div class="am-list-item-text">未确认的菜单</div>
                        <table id="tUnconfirmCart" class="am-table am-table-bd am-table-striped">
                            <thead>
                                <tr>
                                    <th>菜名</th>
                                    <th>单价</th>
                                    <th>份数</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="am-g">
                    <div style="text-align: center;">
                        <button type="button" id="btnClearCart" class="am-btn am-btn-success am-btn-xl" onclick="clearCart();">清空购物车</button>
                        <button type="button" id="btnCreateOrder" class="am-btn am-btn-warning am-btn-xl" onclick="payOrder()"><span class="am-icon-shopping-cart"></span>&nbsp;<span id="sCreateOrder">立即结账</span></button>
                    </div>
                </div>
            </div>
        </div>
        <!-- content end -->

    </div>

    <!-- 订单ID -->
    <input type="hidden" id="hOrderId" runat="server" />

    <!-- “已下单的数据”和“未确认的数据”(json格式) -->
    <input type="hidden" id="hOrderData" runat="server" />
    <input type="hidden" id="hUnconfirmData" runat="server" />


    <!-- Navbar -->
    <div data-am-widget="navbar" class="am-navbar am-cf am-navbar-default" id="divNav">
        <ul class="am-navbar-nav am-cf am-avg-sm-2 am-thumbnails">
            <li>
                <a href="javascript:showMenu();" style="line-height: 49px;">菜单
                </a>
            </li>
            <li>
                <a href="javascript:showCart();" style="line-height: 49px;">购物车，合计&nbsp;<span id="sTotalPrice">0</span>&nbsp;元
                </a>
            </li>
        </ul>
    </div>

    <!-- remark -->
    <div class="am-modal am-modal-no-btn" tabindex="-1" id="divRemark">
        <div class="am-modal-dialog">
            <div class="am-modal-hd">
                <input type="hidden" id="remarkFoodId" />
                <span id="remarkFoodName"></span>
                <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
            </div>
            <div class="am-modal-bd">
                <hr />
                <div style="margin-top: -15px;">
                    <img id="remarkFoodImage" src="" alt="" class="am-img-responsive am-img-thumbnail" style="height: 200px;">
                    <div class="am-form" style="text-align: left;">
                        <div class="am-form-group" style="margin-top: 2px;">
                            <div>口味</div>
                            <div>
                                <button type="button" class="am-btn am-btn-default am-btn-xs" id="btnTaste1" value="1" onclick="setTaste(this);">免辣</button>
                                <button type="button" class="am-btn am-btn-default am-btn-xs" id="btnTaste2" value="2" onclick="setTaste(this);">微辣</button>
                                <button type="button" class="am-btn am-btn-default am-btn-xs" id="btnTaste3" value="3" onclick="setTaste(this);">中辣</button>
                                <button type="button" class="am-btn am-btn-default am-btn-xs" id="btnTaste4" value="4" onclick="setTaste(this);">特辣</button>
                            </div>
                        </div>
                        <div class="am-form-group" style="margin-top: -15px;">
                            <div>备注</div>
                            <div>
                                <input type="text" id="remarkFoodRemark" class="am-input-sm" placeholder="给店家交代点什么" maxlength="50" required>
                            </div>
                        </div>
                        <div class="am-form-group" style="margin-top: -12px;">
                            <button type="button" class="am-btn am-btn-primary am-btn-xs" data-am-modal-close>确定</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        var hUnconfirmData = '<%= this.hUnconfirmData.ClientID %>';
        var hOrderData = '<%= this.hOrderData.ClientID %>';
        var hOrderId = '<%= this.hOrderId.ClientID %>';
        var restaurantId = <%= RestaurantId %>;
        var tableId = <%= TableId %>;        

        //备注窗口关闭事件
        $('#divRemark').on('closed.modal.amui', function () {
            var id = $('#remarkFoodId').val();
            var oUnconfirmData = getJsonObject(hUnconfirmData);
            if (oUnconfirmData[id] === undefined) {
                oUnconfirmData[id] = {};
                oUnconfirmData[id].COUNT = 0;
                oUnconfirmData[id].PRICE = parseFloat($('#liFood' + id).attr('foodprice'));
                oUnconfirmData[id].FOOD_NAME = $('#liFood' + id).attr('foodname');
            }
            oUnconfirmData[id].TASTE = $('.am-active').attr('value');
            oUnconfirmData[id].REMARK = $('#remarkFoodRemark').val();
            $('#' + hUnconfirmData).val(JSON.stringify(oUnconfirmData));
            bindCart();
            updateOrder(id, 'remark');
        });

    </script>

</asp:Content>

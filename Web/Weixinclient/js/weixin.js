function refresh() {
    window.location.reload();
}
function goback() {
    history.go(-1);
}
function gohome() {
    window.location.href = 'WeixinOrderInput.aspx';
}

function goTicketlist() {
    window.location.href = 'TicketList.aspx?v=' + Math.random();
}

function goCenter() {
    window.location.href = 'MemberCenter.aspx';
}



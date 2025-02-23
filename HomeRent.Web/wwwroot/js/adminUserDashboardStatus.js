const adminConnection = new signalR.HubConnectionBuilder()
    .withUrl("/userStatusHub")
    .build();

adminConnection.start().then(function () {
    adminConnection.invoke("GetOnlineUsers").then(function (onlineUsers) {
        const rows = document.querySelectorAll('#usersTable tbody tr');

        rows.forEach(row => {
            const userId = row.getAttribute('data-userId');
            const statusBadge = row.querySelector('.user-status');

            // If the hub reports this user as online, update the button accordingly.
            if (onlineUsers.hasOwnProperty(userId)) {
                statusBadge.textContent = 'Онлайн';
                statusBadge.classList.remove('btn-danger');
                statusBadge.classList.add('btn-success');
            } else {
                statusBadge.textContent = 'Офлайн';
                statusBadge.classList.remove('btn-success');
                statusBadge.classList.add('btn-danger');
            }
        });
    }).catch(function (err) {
        console.error("Error fetching online users: ", err);
    });

    adminConnection.on("UserStatusChanged", function (userId, status) {
        const row = document.querySelector(`#usersTable tbody tr[data-userId="${userId}"]`);

        if (row) {
            const statusBadge = row.querySelector('.user-status');

            if (status === 'Online') {
                statusBadge.textContent = 'Онлайн';
                statusBadge.classList.remove('btn-danger');
                statusBadge.classList.add('btn-success');
            } else if (status === 'Offline') {
                statusBadge.textContent = 'Офлайн';
                statusBadge.classList.remove('btn-success');
                statusBadge.classList.add('btn-danger');
            }
        }
    });
}).catch(function (err) {
    console.error("Error connecting to the hub: ", err);
});
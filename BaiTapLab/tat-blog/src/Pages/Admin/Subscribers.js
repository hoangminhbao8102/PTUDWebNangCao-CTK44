import React, { useEffect, useState } from "react";
import { getSubscribers, deleteSubscriber } from "../../Services/SubscriberRepository";
import { Table, Button } from "react-bootstrap";

const Subscribers = () => {
  const [subscribers, setSubscribers] = useState([]);

  useEffect(() => {
    document.title = "Danh sách người đăng ký";
    fetchSubscribers();
  }, []);

  const fetchSubscribers = () => {
    getSubscribers().then((data) => {
      if (data && data.isSuccess) {
        setSubscribers(data.result);
      }
    });
  };

  const handleDelete = (id) => {
    if (window.confirm("Xác nhận xóa người đăng ký?")) {
      deleteSubscriber(id).then(() => fetchSubscribers());
    }
  };

  return (
    <div className="container mt-4">
      <h2>Danh sách người đăng ký</h2>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Email</th>
            <th>Ngày đăng ký</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {subscribers.map((item) => (
            <tr key={item.id}>
              <td>{item.email}</td>
              <td>{new Date(item.subscribedAt).toLocaleString()}</td>
              <td>
                <Button variant="danger" onClick={() => handleDelete(item.id)}>
                  Xóa
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Subscribers;

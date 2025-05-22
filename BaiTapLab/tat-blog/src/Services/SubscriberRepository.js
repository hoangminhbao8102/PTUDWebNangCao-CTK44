import { get_api, post_api, delete_api } from "./Methods";

export function getSubscribers() {
  return get_api("https://localhost:7239/api/subscribers?PageSize=10&PageNumber=1");
}

export function subscribe(email) {
  return post_api("https://localhost:7239/api/subscribers/subscribe", { email });
}

export function deleteSubscriber(id) {
  return delete_api(`https://localhost:7239/api/subscribers/${id}`);
}

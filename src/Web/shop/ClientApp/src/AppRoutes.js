import { SinglePageProduct } from "./components/SinglePageProduct";
import { Products } from "./components/Products";
import CreateCarProduct from "./components/CreateCarProduct";
import Registration from "./components/Registration";
import Login from "./components/Login";
import UserPasswordChangeAdmin from './components/UserPasswordChangeAdmin'
import EmailVerificationSuccess from "./components/EmailVerificationSuccess";
import EmailVerificationFailure from "./components/EmailVerificationFailed";
import { SubscriptionsPage } from "./components/subscriptions";
import { UsersPage } from "./components/users/UsersPage";
import MyPage from "./components/users/UserPage";
import ReportPage from './components/ReportPage';

const AppRoutes = [
  {
    index: true,
    element: <Products />
  },
  {
    path: '/products/:id',
    element: <SinglePageProduct />
  },
  {
    path: '/products/create',
    element: <CreateCarProduct />
  },
  {
    path: '/registration',
    element: <Registration />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/users',
    element: <UsersPage />
  },
  {
    path: '/users/:id',
    element: <MyPage />
  },
  {
    path: '/users/admin/:id/change-password', 
    element: <UserPasswordChangeAdmin />
  },
  {
    path: '/emailConfirmed',
    element: <EmailVerificationSuccess />
  },
  {
    path: '/emailConfirmationFailed',
    element: <EmailVerificationFailure />
  },
  {
    path: '/subscriptions',
    element: <SubscriptionsPage />
  },
  {
    path: '/report',
    element: <ReportPage />
  }
];

export default AppRoutes;

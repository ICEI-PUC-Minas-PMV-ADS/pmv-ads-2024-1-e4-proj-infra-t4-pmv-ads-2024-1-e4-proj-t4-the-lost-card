import React, {useContext} from 'react';
import AnonRoutes from './anon.routes';
import AuthContext from '../Context/auth';
import AppRoutes from './app.routes';
import { StatusBar } from 'react-native';

StatusBar.setHidden(true);

const Routes: React.FC = () => {
  const {user} = useContext(AuthContext);
  return user ? <AppRoutes /> : <AnonRoutes />;
};

export default Routes;

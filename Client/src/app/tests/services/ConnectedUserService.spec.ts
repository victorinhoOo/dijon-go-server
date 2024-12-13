import { TestBed } from '@angular/core/testing';
import { IObserver } from '../../Model/Observer/IObserver';
import { ConnectedUsersService } from '../../Model/services/connected-users.service';
import { Observable } from '../../Model/Observer/Observable';
import { FakeUserDAO } from '../FakeDAO/FakeUserDAO';


// Classe simulée pour agir en tant qu'observateur
class FakeObserver implements IObserver {
  public updates: Observable[] = [];

  update(object: Observable): void {
    this.updates.push(object);
  }
}

describe('Test ConnectedUsersService', () => {
  let service: ConnectedUsersService;
  let fakeObserver: FakeObserver;
  let dao: FakeUserDAO;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConnectedUsersService],
    });
    dao = new FakeUserDAO();
    service = TestBed.inject(ConnectedUsersService);
    fakeObserver = new FakeObserver();
    service.register(fakeObserver); // Ajoute un observateur au service
  });

  it('devrait être créé', () => {
    expect(service).toBeTruthy();
  });

  it('devrait mettre à jour la liste des utilisateurs connectés et notifier les observateurs', () => {
    const users = dao.getUsers();
    let tab = [users[0].Username,users[1].Username,users[2].Username];
    service.setConnectedUsers(tab);

    // Vérifie que la liste a bien été mise à jour
    expect(service.getConnectedUsers()).toEqual(tab);

    // Vérifie que l'observateur a été notifié
    expect(fakeObserver.updates.length).toBe(1);
    expect(fakeObserver.updates[0]).toBe(service);
  });

  it('ne devrait pas notifier si aucune modification n’est apportée', () => {
    const users = dao.getUsers();
    let initialUsers = [users[0].Username,users[1].Username];
    service.setConnectedUsers(initialUsers);

    // Vérifie la notification initiale
    expect(fakeObserver.updates.length).toBe(1);

    // Réapplique la même liste d'utilisateurs
    service.setConnectedUsers(initialUsers);

    // Vérifie qu'aucune nouvelle notification n'a été envoyée
    expect(fakeObserver.updates.length).toBe(2); // Toujours 2 updates
  });
});
